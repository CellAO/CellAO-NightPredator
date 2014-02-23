CREATE TABLE  `organizations` (
  `Id` int(32) unsigned NOT NULL AUTO_INCREMENT,
  `Creation` datetime NOT NULL,
  `Name` varchar(32) NOT NULL,
  `LeaderID` int(32) NOT NULL,
  `GovernmentForm` int(32) NOT NULL,
  `Description` varchar(255) NOT NULL DEFAULT '" "',
  `Objective` varchar(255) NOT NULL DEFAULT '" "',
  `History` varchar(255) NOT NULL DEFAULT '" "',
  `Tax` int(32) NOT NULL DEFAULT '0',
  `Bank` bigint(20) unsigned NOT NULL DEFAULT '0',
  `Comission` int(32) NOT NULL DEFAULT '1',
  `ContractsID` int(32) NOT NULL DEFAULT '0',
  `CityID` int(32) NOT NULL DEFAULT '0',
  `TowerFieldID` int(32) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;