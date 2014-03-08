CREATE TABLE `items` (
	`Id` INT(32) NOT NULL AUTO_INCREMENT,
	`ContainerType` INT(32) NOT NULL,
	`ContainerInstance` INT(32) NOT NULL,
	`ContainerPlacement` INT(32) NOT NULL,
	`LowId` INT(32) NOT NULL,
	`HighId` INT(32) NOT NULL,
	`Quality` INT(32) NOT NULL,
	`MultipleCount` INT(32) NOT NULL,
	 PRIMARY KEY (`ID`),
	 UNIQUE INDEX `Key1` (`ContainerType`, `ContainerInstance`, `ContainerPlacement`)
)
COMMENT='Non instanced items go here'
COLLATE='latin1_general_ci'
ENGINE=InnoDB;
