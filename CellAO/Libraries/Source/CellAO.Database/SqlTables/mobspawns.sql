CREATE TABLE  `mobspawns` (
  `Id` int(32) NOT NULL,
  `Playfield` int(32) NOT NULL DEFAULT '100',
  `X` float NOT NULL DEFAULT '0',
  `Y` float NOT NULL DEFAULT '0',
  `Z` float NOT NULL DEFAULT '0',
  `HeadingX` float NOT NULL DEFAULT '0',
  `HeadingY` float NOT NULL DEFAULT '0',
  `HeadingZ` float NOT NULL DEFAULT '0',
  `HeadingW` float NOT NULL DEFAULT '0',
  `Name` varchar(256) NOT NULL DEFAULT '',
  `Textures0` int(32) DEFAULT '0',
  `Textures1` int(32) DEFAULT '0',
  `Textures2` int(32) DEFAULT '0',
  `Textures3` int(32) DEFAULT '0',
  `Textures4` int(32) DEFAULT '0',
  `Waypoints` blob,
  `Weaponpairs` blob,
  `RunningNanos` blob,
  `MobMeshs` blob,
  `AdditionalMeshs` blob,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;


--
-- Below is an example set of a few mob spawns, this must be updated during level design.
--

