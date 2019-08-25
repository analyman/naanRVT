import clr

import NaaN.dynamo as xx

# revit assembly
clr.AddReferenceToFileAndPath("C:\Program Files/Autodesk/Revit 2017/RevitAPI.dll")
clr.AddReferenceToFileAndPath("C:\Program Files/Autodesk/Revit 2017/RevitAPIUI.dll")

# dynamo assembly
clr.AddReferenceToFileAndPath("F:\Software/dynamo2.3/DynamoCore.dll")
clr.AddReferenceToFileAndPath("F:\Software/dynamo2.3/DynamoCrypto.dll")
clr.AddReferenceToFileAndPath("F:\Software/dynamo2.3/ProtoGeometry.dll")

import Autodesk.Revit.DB as db
import Autodesk.Revit.UI as ui
import Autodesk.DesignScript.Geometry as dg

class pythonPoint(objct):
    def __init__(self, x, y, z = 0):
        self.x = x
        self.y = y
        self.z = z
    def swapxy(self):
        temp = self.y
        self.y = self.x
        self.x = temp

class DiamondPoints(object):
    def __init__(self, xnum, ynum, xdis, ydis):
        self.xnum = xnum
        self.ynum = ynum
        self.xdis = xdis
        self.ydis = ydis

    def DiamondInfo():
        return [self.xnum, self.ynum, self.xdis, self.ydis]

    def DIA_getPoint(x, y):
        return 