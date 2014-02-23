CREATE TABLE `receivedmsgs` (
	`Id` INT(32) NOT NULL AUTO_INCREMENT,
	`PlayerId` INT(32) NOT NULL,
	`ReceivedId` INT(32) NOT NULL,
	PRIMARY KEY (`Id`),
	INDEX `index1` (`PlayerId`)
)
ENGINE=InnoDB;
