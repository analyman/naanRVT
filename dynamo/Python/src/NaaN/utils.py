import math


def multiLevelMap(func, xlist):
    try:
        extract = list(xlist)
    except TypeError:
        return func(xlist)
    return list(map(lambda x: multiLevelMap(func, x), extract))
