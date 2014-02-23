CREATE TABLE `instanceditems` (
    `ID` int(32) NOT NULL AUTO_INCREMENT, -- PK and item instance

	`ContainerType` INT(32) NOT NULL,
	`ContainerInstance` INT(32) NOT NULL,
	`ContainerPlacement` INT(32) NOT NULL,
	`Itemtype` INT(32) NOT NULL,
	`LowId` INT(32) NOT NULL,
	`HighId` INT(32) NOT NULL,
	`Quality` INT(32) NOT NULL,
	`MultipleCount` INT(32) NOT NULL,
	`X` FLOAT NOT NULL,
	`Y` FLOAT NOT NULL,
	`Z` FLOAT NOT NULL,
	`HeadingX` FLOAT NOT NULL,
	`HeadingY` FLOAT NOT NULL,
	`HeadingZ` FLOAT NOT NULL,
	`HeadingW` FLOAT NOT NULL,
	`stats` BLOB NULL,
	PRIMARY KEY (`Id`),
	UNIQUE INDEX `Key1` (`containertype`, `containerinstance`, `containerplacement`)
)
COLLATE='latin1_general_ci'
ENGINE=InnoDB;
