python3 run_with_input.py \
    -B '**/Map*.asp' -B '**/Common*.asp' -B '**/Strategic*.asp' \
    -p 'StrategicBrain/' \
    -S 'clingo' \
    | python3 output_parser.py -S 'clingo' > out_strategic.asp
