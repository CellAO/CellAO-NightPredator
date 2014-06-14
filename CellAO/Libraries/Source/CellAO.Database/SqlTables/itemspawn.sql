CREATE TABLE `itemspawn` (
	`Id` INT(11) NOT NULL AUTO_INCREMENT,
	`UserId` INT(11) NOT NULL,
	`ItemId` INT(11) NOT NULL,
	`Quality` INT(11) NOT NULL,
	`MultipleCount` INT(11) NOT NULL,
	PRIMARY KEY (`Id`)
)
COMMENT='Used to inform ZoneEngine that an item needs to be spawned.'
COLLATE='latin1_general_ci'
ENGINE=InnoDB;