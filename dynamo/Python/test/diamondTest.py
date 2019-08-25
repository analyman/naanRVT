import sys
import pathmagic
import NaaN.dynamo.Geometry as ndg


def Main():
    diamondA = ndg.DiamondPoints(30, 30, 200, 400)
    print(diamondA.__str__())

# test
if(__name__ == "__main__"):
    Main()
