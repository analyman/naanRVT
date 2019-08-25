import math
from ... import utils as nutils


class pythonPoint:
    def __init__(self, x, y, z=0):
        self.x = x
        self.y = y
        self.z = z

    def swapxy(self):
        temp = self.y
        self.y = self.x
        self.x = temp
        return self

    def __TransformB(self, off, radius):
        if(self.y > off + radius):
            return self
        extraDis = (math.pi / 2 - 1) * radius
        if(self.y < off - extraDis):
            self.z += (extraDis + self.y - off - radius)
            self.y = off
            return self
        offDis = off + radius - self.y
        angle = offDis / radius
        complementAngle = (math.pi / 2) - angle
        self.z -= (off - math.sin(complementAngle) * radius)
        self.y = off + radius * (1 - math.cos(complementAngle))
        return self

    def TransformX(self, RX, RY, RNX, RNY):
        self.__TransformB(RY, RNY)
        self.swapxy()
        self.__TransformB(RX, RNX)
        self.swapxy()
        return self

    def __str__(self):
        return ("Point(X = " + self.x.__str__() + " Y = " +
                self.y.__str__() + " Z = " + self.z.__str__() + ")")
