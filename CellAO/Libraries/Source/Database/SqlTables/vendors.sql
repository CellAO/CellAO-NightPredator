CREATE TABLE `vendors` (
  `ID` int(10) NOT NULL,
  `Playfield` int(10) NOT NULL DEFAULT '100',
  `X` float NOT NULL DEFAULT '0',
  `Y` float NOT NULL DEFAULT '0',
  `Z` float NOT NULL DEFAULT '0',
  `HeadingX` float NOT NULL DEFAULT '0',
  `HeadingY` float NOT NULL DEFAULT '0',
  `HeadingZ` float NOT NULL DEFAULT '0',
  `HeadingW` float NOT NULL DEFAULT '0',
  `Name` varchar(256) NOT NULL DEFAULT '',
  `TemplateID` int(10) NOT NULL DEFAULT '0',
  `Hash` varchar(7) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332496,1180,197.016,5.01,203.013,0,0.709066,0,0.705142,'',90562,'TraCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332497,1180,197.01,5.01,198.96,0,0.698414,0,0.715694,'',90564,'SolCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332495,1180,197.01,5.01,207.01,0,0.712582,0,0.701589,'',90589,'AdvCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332494,1180,200.96,5.01,208.99,0,0.999991,0,0.0042134,'',90588,'AgeCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332493,1180,204.979,5.01,208.99,0,0.999958,0,0.00921434,'',90587,'BurCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332492,1180,209.015,5.01,208.921,0,0.999833,0,-0.018281,'',90586,'DocCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332491,1180,212.872,5.01,208.926,0,0.99986,0,0.0167036,'',90585,'EnfCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332498,1180,216.92,5.01,207.01,0,-0.705697,0,0.708514,'',90579,'EngCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332499,1180,216.929,5.01,203.081,0,-0.709233,0,0.704974,'',90576,'FixCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332500,1180,216.971,5.01,199.046,0,-0.696794,0,0.717272,'',90567,'NanCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332501,1180,217.01,5.01,195.081,0,-0.709238,0,0.704969,'',90571,'MarCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332502,1180,213.034,5.01,193.022,0,0.0115796,0,0.999933,'',90569,'MetCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77332503,1180,209.082,5.01,193.058,0,0.00907633,0,0.999959,'',90574,'GenCB');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398029,1181,205.01,5.01,177.087,0,0.999999,0,-0.00172347,'',93043,'SolCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398028,1181,209.025,5.01,177.039,0,-0.999643,0,0.0267083,'',93041,'TraCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398027,1181,213.01,5.01,177.099,0,0.999999,0,-0.00172869,'',93062,'AdvCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398026,1181,214.99,5.01,173.04,0,-0.713209,0,0.700951,'',93061,'AgeCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398025,1181,214.92,5.01,169.085,0,-0.713206,0,0.700954,'',93058,'BurCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398024,1181,214.99,5.01,165.055,0,-0.713206,0,0.700954,'',93057,'DocCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398023,1181,214.99,5.01,161.016,0,-0.702609,0,0.711576,'',93055,'EnfCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398022,1181,213.055,5.01,157.01,0,0.000942258,0,1,'',93053,'EngCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398021,1181,208.99,5.01,157.01,0,-0.011558,0,0.999933,'',93050,'FixCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398020,1181,205.025,5.01,157.017,0,-0.0115587,0,0.999933,'',93044,'NanCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398019,1181,201.054,5.01,157.037,0,-0.0165593,0,0.999863,'',93049,'MarCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398018,1181,199.01,5.01,161.01,0,0.709244,0,0.704963,'',93047,'MetCA');
INSERT INTO `vendors` (`ID`,`Playfield`,`X`,`Y`,`Z`,`HeadingX`,`HeadingY`,`HeadingZ`,`HeadingW`,`Name`,`TemplateID`,`Hash`) VALUES (77398017,1181,199.01,5.01,164.962,0,0.696792,0,0.717273,'',90574,'GenCB');
