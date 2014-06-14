CREATE TABLE `stats` (
	`Id` INT(32) NOT NULL AUTO_INCREMENT,
	`Instance` INT(32) NOT NULL,
	`Type` INT(32) NOT NULL,
	`StatId` INT(32) NOT NULL,
	`StatValue` INT(32) NOT NULL,
	UNIQUE INDEX `main` (`Type`, `Instance`, `StatId`),
	PRIMARY KEY (`Id`)
)
COLLATE='latin1_general_ci'
ENGINE=InnoDB;
