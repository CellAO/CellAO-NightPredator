CREATE TABLE  `charactersuploadednanos` (
	`Id` int(32) NOT NULL AUTO_INCREMENT,
	`CharacterId` int(32) NOT NULL,

  `Nano` int(11) NOT NULL,
  PRIMARY KEY (`ID`,`Nano`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
