# 将<注释文字>转化为族名为<FModelText>的常规模型族实例, <FModelText>需要有个名为
# <Text>的实例参数用于设置内容

import Autodesk.Revit.DB as db
import Autodesk.Revit.UI as ui

import Autodesk.Revit.DB.Structure as dbs
import Autodesk.Revit.UI.Selection as uis

cdoc = __revit__.ActiveUIDocument.Document
filterTextNotesCollector = FilteredElementCollector(cdoc)
filterTextNotesCollector.OfCategory(BuiltInCategory.OST_TextNotes)
noteTextList = list(map(lambda x: x, filterTextNotesCollector.ToElements()))

allGenericModel = FilteredElementCollector(cdoc)
allGenericModel.OfCategory(BuiltInCategory.OST_GenericModel)
allGMList = list(filter(lambda x: (type(x) == FamilySymbol and x.FamilyName == "FModelText"), allGenericModel))


def noteTextToModelText(noteText):
	insc = cdoc.Create
	print("Try <{0}>".format(noteText.Text))
	newins = insc.NewFamilyInstance(noteText.Coord, allGMList[0], cdoc.GetElement(noteText.OwnerViewId).GenLevel, dbs.StructuralType.NonStructural)
	setInsPara(newins, "Text", noteText.Text)


def setInsPara(elem, ParaName, Value):
	paraList = list(map(lambda x: x, elem.Parameters))
	for para in paraList:
		if(para.Definition.Name == ParaName):
			para.Set(Value)
			return 0

t = Transaction(cdoc, "NoteText to ModelTextFake")
t.Start()
list(map(lambda x: noteTextToModelText(x), noteTextList))
t.Commit()
