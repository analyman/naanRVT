import Autodesk.DesignScript.Geometry as adg
from .pythonPoint import pythonPoint


def pythonPointToDSPoint(pypt):
    return adg.Point.ByCoordinates(pypt.x, pypt.y, pypt.z)


def PointTopythonPoint(pt):
    return pythonPoint(pt.X, pt.Y, pt.Z)
