CREATE TABLE  `characters` (
  `Id` int(32) NOT NULL AUTO_INCREMENT, -- PK

  `Username` varchar(32) NOT NULL,
  `Name` varchar(32) NOT NULL,
  `FirstName` varchar(32) NOT NULL,
  `LastName` varchar(32) NOT NULL,
  `Textures0` int(32) NOT NULL DEFAULT '0',
  `Textures1` int(32) NOT NULL DEFAULT '0',
  `Textures2` int(32) NOT NULL DEFAULT '0',
  `Textures3` int(32) NOT NULL DEFAULT '0',
  `Textures4` int(32) NOT NULL DEFAULT '0',
  `playfield` int(32) NOT NULL,
  `X` float NOT NULL,
  `Y` float NOT NULL,
  `Z` float NOT NULL,
  `HeadingX` float NOT NULL,
  `HeadingY` float NOT NULL,
  `HeadingZ` float NOT NULL,
  `HeadingW` float NOT NULL,
  `Online` smallint(6) DEFAULT '0',

  `BuddyList` varchar(500) NULL DEFAULT '', -- csv list of buddy player ID

  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;