CREATE TABLE `stats` (
	`type` INT(32) NOT NULL,
	`instance` INT(32) NOT NULL,
	`statid` INT(32) NOT NULL,
	`statvalue` INT(32) NOT NULL,
	UNIQUE INDEX `main` (`type`, `instance`, `statid`)
)
COLLATE='latin1_general_ci'
ENGINE=InnoDB;
