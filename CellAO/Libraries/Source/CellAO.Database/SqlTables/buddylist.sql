CREATE TABLE `buddylist` (
	`PlayerID` INT(32) NOT NULL,
	`BuddyID` INT(32) NOT NULL,
	INDEX `index1` (`PlayerID`)
)
ENGINE=InnoDB;
