CREATE TABLE  `mobspawnsinventory` (
  `Id` int(32) NOT NULL,
  `Playfield` int(32) NOT NULL,
  `Placement` int(32) NOT NULL DEFAULT '0',
  `Flags` int(32) NOT NULL DEFAULT '0',
  `MultipleCount` int(32) NOT NULL DEFAULT '0',
  `Type` int(32) NOT NULL DEFAULT '0',
  `Instance` int(32) NOT NULL DEFAULT '0',
  `LowID` int(32) NOT NULL DEFAULT '0',
  `HighID` int(32) NOT NULL DEFAULT '0',
  `Quality` int(32) NOT NULL DEFAULT '0',
  `Nothing` int(32) NOT NULL DEFAULT '0',
  `UniqueId` int(32) NOT NULL AUTO_INCREMENT,
  `container` int(32) NOT NULL,
  PRIMARY KEY (`UniqueId`)
) ENGINE=InnoDB AUTO_INCREMENT=130 DEFAULT CHARSET=latin1;