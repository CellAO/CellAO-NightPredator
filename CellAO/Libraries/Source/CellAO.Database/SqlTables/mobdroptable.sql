CREATE TABLE IF NOT EXISTS `mobdroptable` (
  `Hash` tinytext NOT NULL,
  `LowID` int(11) unsigned NOT NULL,
  `HighID` int(11) unsigned NOT NULL,
  `MinQL` int(10) unsigned NOT NULL,
  `MaxQL` int(10) unsigned NOT NULL,
  `RangeCheck` int(10) unsigned NOT NULL COMMENT '0 = can drop regardless of mob level (insignias, quest items, etc.), 1 = check against mob level'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("INSTDC", 147073, 147073, 80, 80, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("INSTDC", 160840, 160840, 142, 142, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("INSTDC", 147272, 147272, 140, 140, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("INSTDC", 147903, 147903, 80, 80, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("NANOBN", 221566, 221566, 66, 66, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("NANOBN", 220792, 220792, 146, 146, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("NANOBN", 221021, 221021, 14, 14, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("NANOBN", 222114, 222114, 17, 17, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("NANOBN", 220931, 220931, 24, 24, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("NANORK", 163097, 163097, 149, 149, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("NANORK", 203120, 203120, 161, 161, 1); 
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("NANORK", 161160, 161160, 169, 169, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("NANORK", 162725, 162725, 93, 93, 1);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("ABANSG", 214788, 214788, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("THRKSG", 214789, 214789, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("ENELSG", 214781, 214781, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("SHERSG", 214782, 214782, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("OCRASG", 214840, 214840, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("ROCHSG", 214855, 214855, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("GLTHSG", 214880, 214880, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("DALJSG", 214881, 214881, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("CAMASG", 224052, 224052, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("VANYSG", 224049, 224049, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("GALASG", 224051, 224051, 1, 1, 0);
INSERT INTO `mobdroptable` (`Hash`,`LowID`,`HighID`,`MinQL`, `MaxQL`,`RangeCheck`) VALUES  ("MORDSG", 224050, 224050, 1, 1, 0);