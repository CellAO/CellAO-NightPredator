CREATE TABLE IF NOT EXISTS `expansions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Value` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=12 ;


TRUNCATE TABLE `expansions`;

INSERT INTO `expansions` (`Id`, `Name`, `Value`) VALUES
(1, 'Notum Wars', 1),
(2, 'Shadow Lands', 2),
(3, 'Shadow Lands Pre-Order', 4),
(4, 'Alien Invasion', 8),
(5, 'Alien Invasion Pre-Order', 16),
(6, 'Lost Eden', 32),
(7, 'Lost Eden Pre-Order', 64),
(8, 'Legacy of Xan', 128),
(9, 'Legacy of Xan Pre-Order', 256),
(10, 'Mail', 512),
(11, 'PMV Obsidian Edition', 1024);
