CREATE TABLE `vendors` (
  `Id` int(32) NOT NULL,
  `Playfield` int(32) NOT NULL DEFAULT '100',
  `X` float NOT NULL DEFAULT '0',
  `Y` float NOT NULL DEFAULT '0',
  `Z` float NOT NULL DEFAULT '0',
  `HeadingX` float NOT NULL DEFAULT '0',
  `HeadingY` float NOT NULL DEFAULT '0',
  `HeadingZ` float NOT NULL DEFAULT '0',
  `HeadingW` float NOT NULL DEFAULT '0',
  `Name` varchar(256) NOT NULL DEFAULT '',
  `TemplateId` int(32) NOT NULL DEFAULT '0',
  `Hash` varchar(7) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Below is an example set of a few vendors, this must be updated during level design.
--

INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332492, 1180, 197, 5, 203, 0, 0.707108, 0, 0.707106, '', 90562, 'TraCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332491, 1180, 197, 5, 199, 0, 0.707108, 0, 0.707106, '', 90564, 'SolCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332480, 1180, 197, 5.00014, 207, 0, 0.707108, 0, 0.707106, '', 90589, 'AdvCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332481, 1180, 201, 5.00014, 209, 0, 1, 0, -0.00000361999, '', 90588, 'AgeCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332482, 1180, 205, 5.00004, 209, 0, 1, 0, -0.00000361999, '', 90587, 'BurCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332483, 1180, 209, 5.00005, 209, 0, 1, 0, -0.00000361999, '', 90586, 'DocCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332484, 1180, 213, 5.00011, 209, 0, 1, 0, -0.00000361999, '', 90585, 'EnfCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332485, 1180, 217, 5, 207, 0, -0.707103, 0, 0.707111, '', 90579, 'EngCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332486, 1180, 217, 5.00054, 203, 0, -0.707103, 0, 0.707111, '', 90576, 'FixCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332490, 1180, 217, 5, 199, 0, -0.707103, 0, 0.707111, '', 90567, 'NanCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332488, 1180, 217, 5.00021, 195, 0, -0.707103, 0, 0.707111, '', 90571, 'MarCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332489, 1180, 213, 5.0002, 193, 0, 0, 0, 1, '', 90569, 'MetCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77332487, 1180, 209, 5, 193, 0, 0, 0, 1, '', 90574, 'GenCB');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398029, 1181, 177, 5, 167, 0, 0.707108, 0, 0.707106, '', 93043, 'SolCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398028, 1181, 209, 5, 177, 0, 1, 0, -0.00000149012, '', 93041, 'TraCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398027, 1181, 205, 5, 177, 0, 1, 0, -0.00000149012, '', 93062, 'AdvCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398026, 1181, 205, 5, 157, 0, 0.00000599027, 0, 1, '', 93061, 'AgeCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398025, 1181, 199, 5.0002, 161, 0, 0.707107, 0, 0.707107, '', 93058, 'BurCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398024, 1181, 201, 5.00021, 157, 0, 0.00000599027, 0, 1, '', 93057, 'DocCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398023, 1181, 199, 5, 165, 0, 0.707107, 0, 0.707107, '', 93055, 'EnfCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398022, 1181, 209, 5.00054, 157, 0, 0.00000599027, 0, 1, '', 93053, 'EngCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398021, 1181, 213, 5, 157, 0, 0.00000599027, 0, 1, '', 93050, 'FixCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398020, 1181, 215, 5.00011, 161, 0, -0.707104, 0, 0.707109, '', 93044, 'NanCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398019, 1181, 215, 5.00005, 165, 0, -0.707104, 0, 0.707109, '', 93049, 'MarCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398018, 1181, 215, 5.00004, 169, 0, -0.707104, 0, 0.707109, '', 93047, 'MetCA');
INSERT INTO `vendors` (`Id`, `Playfield`, `X`, `Y`, `Z`, `HeadingX`, `HeadingY`, `HeadingZ`, `HeadingW`, `Name`, `TemplateId`, `Hash`) VALUES (77398017, 1181, 215, 5.00014, 173, 0, -0.707104, 0, 0.707109, '', 90574, 'GenCB');
