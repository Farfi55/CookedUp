python3 run_with_input.py \
    -B '**/Map*.asp' -B '**/Common*.asp' -B '**/Strategic*.asp' \
    -p 'StrategicBrain/' \
    -S 'clingo' \
    2>&1 | python3 output_parser.py -S 'clingo'
