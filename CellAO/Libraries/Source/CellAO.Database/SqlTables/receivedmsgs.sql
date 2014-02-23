CREATE TABLE `receivedmsgs` (
	`PlayerId` INT(32) NOT NULL,
	`ReceivedId` INT(32) NOT NULL,
	INDEX `index1` (`PlayerId`)
)
ENGINE=InnoDB;
