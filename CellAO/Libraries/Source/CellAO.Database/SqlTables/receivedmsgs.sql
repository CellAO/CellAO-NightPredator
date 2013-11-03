CREATE TABLE `receivedmsgs` (
	`PlayerID` INT(32) NOT NULL,
	`ReceivedID` INT(32) NOT NULL,
	INDEX `index1` (`PlayerID`)
)
ENGINE=InnoDB;
