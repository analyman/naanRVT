import os
import sys

gm_directory = os.path.dirname(os.path.realpath(
    os.path.dirname(__file__))).split(os.sep)
sys.path.insert(0, os.sep.join(gm_directory + ["src"]))
