import sys

allow_list = []
allow_list_prefixes = []
deny_list = []
deny_list_prefixes = []

solver = 'dlv'

skip_log_prefixes = []

i = 1
while i < len(sys.argv):
    if sys.argv[i] in ['-a', '--allow']:
        allow_list.append(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-A', '--allow-prefix']:
        allow_list_prefixes.append(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-d', '--deny']:
        deny_list.append(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-D', '--deny-prefix']:
        deny_list_prefixes.append(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-S', '--solver']:
        solver = sys.argv[i + 1].lower()
        i += 1
    elif sys.argv[i] in ['-l', '--skip-log-prefix']:
        skip_log_prefixes.append(sys.argv[i + 1])
        i += 1
    elif sys.argv[i] in ['-h', '--help']:
        print(f"Usage: python output_parser.py")
        print(f"Options:")
        print(f"  -h, --help: show this help")
        print(f"  -a, --allow: allow predicate")
        print(f"  -A, --allow-prefix: allow predicate prefix")
        print(f"  -d, --deny: deny predicate")
        print(f"  -D, --deny-prefix: deny predicate prefix")
        print(f"  -S, --solver: solver (dlv, clingo)")
        print(f"  -l, --skip-log-prefix: skip log prefix")
        exit(0)
    i += 1

if solver not in ['dlv', 'clingo']:
    print(f"Unknown solver: {solver}")
    exit(1)

uses_allow_list = len(allow_list) > 0 or len(allow_list_prefixes) > 0
uses_deny_list = len(deny_list) > 0 or len(deny_list_prefixes) > 0

if not uses_allow_list and not uses_deny_list:
    deny_list_prefixes.append('s_')
    deny_list_prefixes.append('c_')
    deny_list_prefixes.append('tmp_')
    uses_deny_list = True

if skip_log_prefixes == []:
    skip_log_prefixes.append('s_')
    skip_log_prefixes.append('a_')


is_reading_clingo_logs = solver == 'clingo'
clingo_log_line2 = 'N/A'
clingo_log_line1 = 'N/A'
formatted_clingo_log_messages = []


output = ''
input_line = ''
prev_line = ''


while(True):
    prev_line = input_line
    input_line = input()

    if input_line == 'END':
        break

    if input_line.startswith('START'):
        is_reading_clingo_logs = False
        continue


    if solver == 'dlv':
        is_predicate_line = input_line.startswith('{') and input_line.endswith('}')
    elif solver == 'clingo':
        is_predicate_line = prev_line.startswith('Answer')

    if is_reading_clingo_logs:
        clingo_log_line2 = clingo_log_line1
        clingo_log_line1 = prev_line.strip()

        if input_line == '' and clingo_log_line1 != '':
            if clingo_log_line2 != '':
                clingo_log_parts = clingo_log_line2.split(':')
                clingo_log_predicate = clingo_log_line1
            else:
                clingo_log_parts = clingo_log_line1.split(':')
                clingo_log_predicate = ''
            clingo_log_file = ':'.join(clingo_log_parts[:3]).strip()
            clingo_log_type = clingo_log_parts[3].strip()
            clingo_log_message = ':'.join(clingo_log_parts[4:]).strip()
            
            skip_log = False

            if clingo_log_type == 'info':
                for skip_log_prefix in skip_log_prefixes:
                    if clingo_log_predicate != '' and \
                        clingo_log_predicate.startswith(skip_log_prefix):
                        skip_log = True
                        break
            

            if not skip_log:
                parts = [clingo_log_type, clingo_log_file, clingo_log_message]
                if clingo_log_predicate != '':
                    parts.append(clingo_log_predicate)

                formatted_clingo_log_messages.append(parts)
            clingo_log_line2 = ''
            clingo_log_line1 = ''
        continue
        



    if not is_predicate_line:
        output += input_line + '\n'
        continue

    if solver == 'dlv':
        input_line = input_line[1:-1].replace('), ', ').\n')
    elif solver == 'clingo':
        input_line = input_line.replace(') ', ').\n')

    lines = input_line.splitlines()
    predicates = []

    for predicate in lines:

        
        predicate_name = predicate.split('(')[0]
        
        if predicate_name in deny_list:
            can_add_predicate = False
        elif predicate_name in allow_list:
            can_add_predicate = True
        else:
            can_add_predicate = True
            for deny_prefix in deny_list_prefixes:
                if predicate_name.startswith(deny_prefix):
                    can_add_predicate = False
                    break
            for allow_prefix in allow_list_prefixes:
                if predicate_name.startswith(allow_prefix):
                    can_add_predicate = True
                    break            

        if can_add_predicate:
            predicates.append(predicate)
         
    predicates.sort()
    output += '{\n' + '\n'.join(predicates) + '\n}\n'

USE_COLORS = False
PRINT_LOGS_TO_STDERR = True
if len(formatted_clingo_log_messages) > 0:

    if USE_COLORS:
        import os
        from termcolor import colored
        if os.name == 'nt':
            os.system('color')
        
    print('clingo logs:')
    for parts in formatted_clingo_log_messages:
        clingo_log_type, clingo_log_file, clingo_log_message = parts[0:3]
        clingo_log_predicate = parts[3] if len(parts) > 3 else ''
        
        ftm_log_type = clingo_log_type.upper()
        if USE_COLORS:
            if clingo_log_type == 'info':
                ftm_log_type = colored(ftm_log_type, 'light_blue')
            elif clingo_log_type == 'warning':
                ftm_log_type = colored(ftm_log_type, 'yellow')
            elif clingo_log_type == 'error':
                ftm_log_type = colored(ftm_log_type, 'red')

        print(f"[{ftm_log_type}]: {clingo_log_file}")
        print(f"\t{clingo_log_message}")
        if clingo_log_predicate != '':
            print(f'\t{clingo_log_predicate}')
        print()

        if PRINT_LOGS_TO_STDERR:
            print(f"[{ftm_log_type}]: {clingo_log_file}", file=sys.stderr)
            print(f"\t{clingo_log_message}", file=sys.stderr)
            if clingo_log_predicate != '':
                print(f'\t{clingo_log_predicate}', file=sys.stderr)
            print(file=sys.stderr)

    
print(output)
