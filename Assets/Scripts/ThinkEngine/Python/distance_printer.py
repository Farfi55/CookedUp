from math import sqrt, ceil

MAX_X = 20
MAX_Y = 20

for x in range(MAX_X + 1):
    for y in range(MAX_Y + 1):
        dist = ceil(sqrt(x**2 + y**2) * 1000)
        print(f'c_Distance({x},{y},{dist}).')
