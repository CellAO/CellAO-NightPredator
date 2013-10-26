CREATE TABLE  `organizations` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `creation` datetime NOT NULL,
  `Name` varchar(32) NOT NULL,
  `LeaderID` int(11) NOT NULL,
  `GovernmentForm` int(11) NOT NULL,
  `Description` varchar(255) NOT NULL DEFAULT '" "',
  `Objective` varchar(255) NOT NULL DEFAULT '" "',
  `History` varchar(255) NOT NULL DEFAULT '" "',
  `Tax` int(11) NOT NULL DEFAULT '0',
  `Bank` bigint(20) unsigned NOT NULL DEFAULT '0',
  `Comission` int(11) NOT NULL DEFAULT '1',
  `ContractsID` int(11) NOT NULL DEFAULT '0',
  `CityID` int(11) NOT NULL DEFAULT '0',
  `TowerFieldID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;