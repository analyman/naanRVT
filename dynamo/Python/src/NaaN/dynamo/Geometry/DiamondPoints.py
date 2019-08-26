import math
from ... import utils as nutils
from .pythonPoint import pythonPoint


class DiamondPoints(object):
    def __init__(self, xnum, ynum, xdis, ydis):
        self.xnum = xnum
        self.ynum = ynum
        self.xdis = xdis
        self.ydis = ydis
        self.twoLayerPoints = self.__genDiamondLayer_T1()
        self.AllPanels = self.__DIAGET()
        self.DiamondOuter = self.AllPanels[0]
        self.DiamondInner = self.AllPanels[1]
        self.TriangleA = self.AllPanels[2]
        self.TriangleB = self.AllPanels[3]
        self.TriangleC = self.AllPanels[4]
        self.TriangleD = self.AllPanels[5]

    def DiamondInfo(self):
        return [self.xnum, self.ynum, self.xdis, self.ydis]

    def __DIA_getPoint(self, x, y):
        return pythonPoint(x, y, 0)

    def __DIA_genlayer(self, xnum, ynum, xdis, ydis, offset):
        ret = []
        for i in range(0, xnum * xdis, xdis):
            xlayer = []
            for j in range(0, ynum * ydis, ydis):
                t = self.__DIA_getPoint(i + offset.x, j + offset.y)
                xlayer.append(t)
            ret.append(xlayer)
        return ret

    def __genDiamondLayer_T1(self):
        halfwidth = self.xdis / 2
        halfheight = self.ydis / 2
        firstLayer = self.__DIA_genlayer(self.xnum, self.ynum, self.xdis,
                                         self.ydis, pythonPoint(0, 0))
        secondLayer = self.__DIA_genlayer(self.xnum - 1, self.ynum - 1,
                                          self.xdis, self.ydis,
                                          pythonPoint(halfwidth,
                                                      halfheight))
        return [firstLayer, secondLayer]

    def __DIA_getTriangles(self, TP):
        fl = TP[0]
        sl = TP[1]
        triangleA = []
        triangleB = []
        for i in range(0, self.xnum - 1):
            triangleA.append([fl[i][0], fl[i+1][0], sl[i][0]])
            triangleB.append([fl[i][self.ynum-1], fl[i+1][self.ynum-1],
                              sl[i][self.ynum-2]])
        triangleC = []
        triangleD = []
        for i in range(0, self.ynum - 1):
            triangleC.append([fl[self.xnum-1][i], fl[self.xnum-1][i+1],
                              sl[self.xnum-2][i]])
            triangleD.append([fl[0][i], fl[0][i+1], sl[0][i]])
        return [triangleA, triangleB, triangleC, triangleD]

    def __DIA_getDiamond(self, TP):
        fl = TP[0]
        sl = TP[1]
        diamondA = []
        diamondB = []
        for i in range(0, self.ynum - 1):
            for j in range(0, self.xnum - 1):
                diamondA.append([fl[j][i], sl[j-1][i], fl[j][i+1], sl[j][i]])
        for i in range(0, self.xnum - 1):
            for j in range(1, self.ynum - 1):
                diamondB.append([fl[i][j], sl[i][j-1], fl[i+1][j], sl[i][j]])
        return [diamondA, diamondB]

    def __DIAGET(self):
        return (self.__DIA_getDiamond(self.twoLayerPoints) +
                self.__DIA_getTriangles(self.twoLayerPoints))

    def TransformPoints(self, RX, RY, RateX=0.3, RateY=0.3):
        if(RateX < 0 or RateX > 1 or RateY < 0 or RateY > 1):
            raise Exception("RateX and RateY must in interval [0, 1]")
        nutils.multiLevelMap(
            lambda x: x.TransformX(RX, RY,
                                   RateX * self.xdis * self.xnum,
                                   RateY * self.ydis * self.ynum),
            self.twoLayerPoints)
        return self

    def DefaultTransformPoints(self):
        return self.TransformPoints(self.xdis * 4, self.ydis * 4)

    def __repr__(self):
        return (("{0}.DiamondPoints(xnum = {1}, " +
                 "ynum = {2}, xdis = {3}, ydis = {4})").
                format(
                __package__,
                self.xnum.__repr__(),
                self.ynum.__repr__(),
                self.xdis.__repr__(),
                self.ydis.__repr__()))

    def __str__(self):
        return str(nutils.multiLevelMap(
            lambda x: x.__str__(),
            self.DefaultTransformPoints().AllPanels))
