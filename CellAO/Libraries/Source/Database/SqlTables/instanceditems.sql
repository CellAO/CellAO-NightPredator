CREATE TABLE `instanceditems` (
	`containertype` INT(32) NOT NULL,
	`containerinstance` INT(32) NOT NULL,
	`containerplacement` INT(32) NOT NULL,
	`itemtype` INT(10) NOT NULL,
	`iteminstance` INT(10) NOT NULL,
	`lowid` INT(32) NOT NULL,
	`highid` INT(32) NOT NULL,
	`quality` INT(32) NOT NULL,
	`multiplecount` INT(10) NOT NULL,
	`x` FLOAT NOT NULL,
	`y` FLOAT NOT NULL,
	`z` FLOAT NOT NULL,
	`headingx` FLOAT NOT NULL,
	`headingy` FLOAT NOT NULL,
	`headingz` FLOAT NOT NULL,
	`headingw` FLOAT NOT NULL,
	`stats` BLOB NULL,
	UNIQUE INDEX `Key1` (`containertype`, `containerinstance`, `containerplacement`)
)
COLLATE='latin1_general_ci'
ENGINE=InnoDB;
