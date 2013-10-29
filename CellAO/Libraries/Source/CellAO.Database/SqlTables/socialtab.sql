CREATE TABLE  `socialtab` (
  `charid` int(10) unsigned NOT NULL,
  `Texture1` int(10) NOT NULL,
  `Texture2` int(10) NOT NULL,
  `Texture3` int(10) NOT NULL,
  `Texture4` int(10) NOT NULL,
  `Texture5` int(10) NOT NULL,
  `BackMesh` int(10) NOT NULL,
  `ShoulderMeshLeft` int(10) NOT NULL,
  `ShoulderMeshRight` int(10) NOT NULL,
  `HeadMesh` int(10) NOT NULL,
  `HairMesh` int(10) NOT NULL,
  `WeaponMeshRight` int(10) NOT NULL,
  `WeaponMeshLeft` int(10) NOT NULL,
  PRIMARY KEY (`charid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
