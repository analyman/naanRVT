import sys
import os.path

import clr
clr.AddReference('ProtoGeometry')
import Autodesk.DesignScript.Geometry as adg

sys.path = sys.path + list(
    map(lambda x: os.path.abspath(x),
        filter(lambda x: os.path.exists(x),
               os.environ["IRONPYTHONPATH"].split(';'))))

import NaaN.utils as nutils
import NaaN.dynamo.Geometry as ndg

# <IN: list> hold the input parameters
# <OUT> is the output node

# Code ...
