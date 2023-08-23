python3 run_with_input.py \
    -B '**/Map*.asp' -B '**/Common*.asp' -B '**/Player*.asp' \
    -p 'Player/' \
    -S 'dlv' \
    -o '-n 0' \
    | python3 output_parser.py -S 'dlv' > out_player.asp
