from math import sqrt, ceil
MAX_X_DIFF = 20
MAX_Y_DIFF = 20
for x_diff in range(MAX_X_DIFF + 1):
    for y_diff in range(MAX_Y_DIFF + 1):
        dist = ceil(sqrt(x_diff**2 + y_diff**2) * 1000)
        print(f'c_Distance({x_diff},{y_diff},{dist}).')
