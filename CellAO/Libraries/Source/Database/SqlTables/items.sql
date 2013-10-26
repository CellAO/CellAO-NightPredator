CREATE TABLE `items` (
	`containertype` INT(10) NOT NULL,
	`containerinstance` INT(10) NOT NULL,
	`containerplacement` INT(10) NOT NULL,
	`lowid` INT(10) NOT NULL,
	`highid` INT(10) NOT NULL,
	`quality` INT(10) NOT NULL,
	`multiplecount` INT(10) NOT NULL,
	UNIQUE INDEX `Key1` (`containertype`, `containerinstance`, `containerplacement`)
)
COMMENT='Non instanced items go here'
COLLATE='latin1_general_ci'
ENGINE=InnoDB;
