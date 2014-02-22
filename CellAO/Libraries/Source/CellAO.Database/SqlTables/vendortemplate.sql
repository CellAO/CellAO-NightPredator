CREATE TABLE `vendortemplate` (
  `Hash` varchar(7) NOT NULL,
  `Lvl` int(10) NOT NULL DEFAULT '1',
  `Name` varchar(256) NOT NULL DEFAULT "",
  `itemtemplate` int(10) NOT NULL DEFAULT '0',
  `ShopInvHash` varchar(4) NOT NULL,
  `minQL` int(11) DEFAULT '1',
  `maxQL` int(11) DEFAULT '1',
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `buy` float(3,2) NOT NULL default '0.05', -- Price Modifiere Sell item to Shop
  `sell` float(3,2) NOT NULL default '1.00', -- Price Modifiere Buy item from Shop
  `skill` int(3) DEFAULT '161', -- Skill that change the Basic Price of a Item CompLiter(161) for Machines, Psychology(162) for Humans (SL Garden Shops)
  PRIMARY KEY (`ID`,`Hash`) USING BTREE
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- This list is not complete and should be filled up.
--

insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AdvNB', 1, 'Basic Adventurer Crystals', 43580, 'AdvN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AgeNB', 1, 'Basic Agent Crystals', 43579, 'AgeN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BurNB', 1, 'Basic Bureaucrat Crystals', 43578, 'BurN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('DocNB', 1, 'Basic Doctor Crystals', 43577, 'DocN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EnfNB', 1, 'Basic Enforcer Crystals', 43581, 'EnfN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EngNB', 1, 'Basic Engineer Crystals', 43576, 'EngN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FixNB', 1, 'Basic Fixer Crystals', 43571, 'FixN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('GenNB', 1, 'Basic General Crystals', 43569, 'GenN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('KeeNB', 1, 'Basic Keeper Crystals', 222945, 'KeeN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MarNB', 1, 'Basic Martial Artist Crystals', 43575, 'MarN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MetNB', 1, 'Basic Meta-Physicist Crystals', 43574, 'MetN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('NanNB', 1, 'Basic Nanotechnician Crystals', 43573, 'NanN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('ShaNB', 1, 'Basic Shade Crystals', 222946, 'ShaN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SolNB', 1, 'Basic Soldier Crystals', 43572, 'SolN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('TraNB', 1, 'Basic Trader Crystals', 43570, 'TraN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AdvNA', 1, 'Advanced Adventurer Crystals', 46522, 'AdvN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AgeNA', 1, 'Advanced Agent Crystals', 46520, 'AgeN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BurNA', 1, 'Advanced Bureaucrat Crystals', 46506, 'BurN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('DocNA', 1, 'Advanced Doctor Crystals', 46340, 'DocN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EnfNA', 1, 'Advanced Enforcer Crystals', 46517, 'EnfN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EngNA', 1, 'Advanced Engineer Crystals', 46339, 'EngN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FixNA', 1, 'Advanced Fixer Crystals', 46519, 'FixN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('KeeNA', 1, 'Advanced Keeper Crystals', 222947, 'KeeN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MarNA', 1, 'Advanced Martial Artists Crystals', 46510, 'MarN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MetNA', 1, 'Advanced Meta-Physicist Crystals', 46338, 'MetN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('NanNA', 1, 'Advanced Nanotechnician Crystals', 46337, 'NanN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('ShaNA', 1, 'Advanced Shade Crystals', 222948, 'ShaN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SolNA', 1, 'Advanced Soldier Crystals', 46521, 'SolN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('TraNA', 1, 'Advanced Trader Crystals', 46518, 'TraN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AdvNS', 1, 'Superior Adventurer Crystals', 46516, 'AdvN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AgeNS', 1, 'Superior Agent Crystals', 46515, 'AgeN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BurNS', 1, 'Superior Bureaucrat Crystals', 46507, 'BurN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('DocNS', 1, 'Superior Doctor Crystals', 46336, 'DocN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EnfNS', 1, 'Superior Enforcer Crystals', 46511, 'EnfN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EngNS', 1, 'Superior Engineer Crystals', 46335, 'EngN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FixNS', 1, 'Superior Fixer Crystals', 46512, 'FixN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('KeeNS', 1, 'Superior Keeper Crystals', 222949, 'KeeN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MarNS', 1, 'Superior Martial Artist Crystals', 46509, 'MarN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MetNS', 1, 'Superior Meta-Physicist Crystals', 46334, 'MetN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('NanNS', 1, 'Superior Nanotechnician Crystals', 46341, 'NanN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('ShaNS', 1, 'Superior Shade Crystals', 222950, 'ShaN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SolNS', 1, 'Superior Soldier Crystals', 46514, 'SolN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('TraNS', 1, 'Superior Trader Crystals', 46513, 'TraN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AdvCA', 1, 'Advanced Clan Adventurer Crystals', 93062, 'AdvN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AgeCA', 1, 'Advanced Clan Agent Crystals', 93061, 'AgeN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BurCA', 1, 'Advanced Clan Bureaucrat Crystals', 93058, 'BurN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('DocCA', 1, 'Advanced Clan Doctor Crystals', 93057, 'DocN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EnfCA', 1, 'Advanced Clan Enforcer Crystals', 93055, 'EnfN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EngCA', 1, 'Advanced Clan Engineer Crystals', 93053, 'EngN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FixCA', 1, 'Advanced Clan Fixer Crystals', 93050, 'FixN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MarCA', 1, 'Advanced Clan Martial Artists Crystals', 93049, 'MarN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MetCA', 1, 'Advanced Clan Meta-Physicist Crystals', 93047, 'MetN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('NanCA', 1, 'Advanced Clan Nanotechnician Crystals', 93044, 'NanN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SolCA', 1, 'Advanced Clan Soldier Crystals', 93043, 'SolN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('TraCA', 1, 'Advanced Clan Trader Crystals', 93041, 'TraN', 40, 100, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AdvCB', 1, 'Basic Clan Adventurer Crystals', 90589, 'AdvN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AgeCB', 1, 'Basic Clan Agent Crystals', 90588, 'AgeN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BurCB', 1, 'Basic Clan Bureaucrat Crystals', 90587, 'BurN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('DocCB', 1, 'Basic Clan Doctor Crystals', 90586, 'DocN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EnfCB', 1, 'Basic Clan Enforcer Crystals', 90585, 'EnfN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EngCB', 1, 'Basic Clan Engineer Crystals', 90579, 'EngN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FixCB', 1, 'Basic Clan Fixer Crystals', 90576, 'FixN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('GenCB', 1, 'Basic Clan General Crystals', 90574, 'GenN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MarCB', 1, 'Basic Clan Martial Artist Crystals', 90571, 'MarN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MetCB', 1, 'Basic Clan Meta-Physicist Crystals', 90569, 'MetN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('NanCB', 1, 'Basic Clan Nanotechnician Crystals', 90567, 'NanN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SolCB', 1, 'Basic Clan Soldier Crystals', 90564, 'SolN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('TraCB', 1, 'Basic Clan Trader Crystals', 90562, 'TraN', 1, 50, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AdvCS', 1, 'Superior Clan Adventurer Crystals', 93104, 'AdvN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AgeCS', 1, 'Superior Clan Agent Crystals', 93098, 'AgeN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BurCS', 1, 'Superior Clan Bureaucrat Crystals', 93102, 'BurN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('DocCS', 1, 'Superior Clan Doctor Crystals', 93096, 'DocN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EnfCS', 1, 'Superior Clan Enforcer Crystals', 93091, 'EnfN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EngCS', 1, 'Superior Clan Engineer Crystals', 93093, 'EngN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FixCS', 1, 'Superior Clan Fixer Crystals', 93088, 'FixN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MarCS', 1, 'Superior Clan Martial Artist Crystals', 93090, 'MarN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MetCS', 1, 'Superior Clan Meta-Physicist Crystals', 93085, 'MetN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('NanCS', 1, 'Superior Clan Nanotechnician Crystals', 93086, 'NanN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SolCS', 1, 'Superior Clan Soldier Crystals', 93087, 'SolN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('TraCS', 1, 'Superior Clan Trader Crystals', 93082, 'TraN', 80, 120, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AdvOA', 1, 'Advanced Omni-Tek Adventurer Crystals', 93063, 'AdvN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AgeOA', 1, 'Advanced Omni-Tek Agent Crystals', 93060, 'AgeN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BurOA', 1, 'Advanced Omni-Tek Bureaucrat Crystals', 93059, 'BurN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('DocOA', 1, 'Advanced Omni-Tek Doctor Crystals', 93056, 'DocN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EnfOA', 1, 'Advanced Omni-Tek Enforcer Crystals', 93054, 'EnfN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EngOA', 1, 'Advanced Omni-Tek Engineer Crystals', 93052, 'EngN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FixOA', 1, 'Advanced Omni-Tek Fixer Crystals', 93051, 'FixN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MarOA', 1, 'Advanced Omni-Tek Martial Artists Crystals', 93048, 'MarN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MetOA', 1, 'Advanced Omni-Tek Meta-Physicist Crystals', 93046, 'MetN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('NanOA', 1, 'Advanced Omni-Tek Nanotechnician Crystals', 93045, 'NanN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SolOA', 1, 'Advanced Omni-Tek Soldier Crystals', 93042, 'SolN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('TraOA', 1, 'Advanced Omni-Tek Trader Crystals', 93040, 'TraN', 40, 100, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AdvOB', 1, 'Basic Omni-Tek Adventurer Crystals', 90590, 'AdvN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AgeOB', 1, 'Basic Omni-Tek Agent Crystals', 90580, 'AgeN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BurOB', 1, 'Basic Omni-Tek Bureaucrat Crystals', 90581, 'BurN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('DocOB', 1, 'Basic Omni-Tek Doctor Crystals', 90582, 'DocN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EnfOB', 1, 'Basic Omni-Tek Enforcer Crystals', 90583, 'EnfN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EngOB', 1, 'Basic Omni-Tek Engineer Crystals', 90577, 'EngN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FixOB', 1, 'Basic Omni-Tek Fixer Crystals', 90575, 'FixN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('GenOB', 1, 'Basic Omni-Tek General Crystals', 90573, 'GenN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MarOB', 1, 'Basic Omni-Tek Martial Artist Crystals', 90570, 'MarN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MetOB', 1, 'Basic Omni-Tek Meta-Physicist Crystals', 90568, 'MetN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('NanOB', 1, 'Basic Omni-Tek Nanotechnician Crystals', 90565, 'NanN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SolOB', 1, 'Basic Omni-Tek Soldier Crystals', 90563, 'SolN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('TraOB', 1, 'Basic Omni-Tek Trader Crystals', 90561, 'TraN', 1, 50, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AdvOS', 1, 'Superior Omni-Tek Adventurer Crystals', 93105, 'AdvN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('AgeOS', 1, 'Superior Omni-Tek Agent Crystals', 93099, 'AgeN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BurOS', 1, 'Superior Omni-Tek Bureaucrat Crystals', 93095, 'BurN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('DocOS', 1, 'Superior Omni-Tek Doctor Crystals', 93097, 'DocN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EnfOS', 1, 'Superior Omni-Tek Enforcer Crystals', 93092, 'EnfN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('EngOS', 1, 'Superior Omni-Tek Engineer Crystals', 93094, 'EngN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FixOS', 1, 'Superior Omni-Tek Fixer Crystals', 93089, 'FixN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MarOS', 1, 'Superior Omni-Tek Martial Artist Crystals', 93084, 'MarN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('MetOS', 1, 'Superior Omni-Tek Meta-Physicist Crystals', 93079, 'MetN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('NanOS', 1, 'Superior Omni-Tek Nanotechnician Crystals', 93080, 'NanN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SolOS', 1, 'Superior Omni-Tek Soldier Crystals', 93081, 'SolN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('TraOS', 1, 'Superior Omni-Tek Trader Crystals', 93083, 'TraN', 80, 120, 0.05, 1.00, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BNCO', 1, 'OT Basic Nano Clusters', 118281, 'NaCl', 1, 50, 0.06, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('ANCO', 1, 'OT Advanced Nano Clusters', 118282, 'NaCl', 50, 90, 0.06, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SNCO', 1, 'OT Superior Nano Clusters', 118283, 'NaCl', 90, 200, 0.06, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BNCC', 1, 'Clan Basic Nano Clusters', 118284, 'NaCl', 1, 50, 0.04 ,1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('ANCC', 1, 'Clan Advanced Nano Clusters', 118285, 'NaCl', 50, 90, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SNCC', 1, 'Clan Superior Nano Clusters', 118286, 'NaCl', 90, 200, 0.04, 1.05, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FNCB', 1, 'Faded Nano Clusters - Basic', 155299, 'CluF', 1, 50, 0.04, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FNCA', 1, 'Faded Nano Clusters - Advanced', 155300, 'CluF', 50, 90, 0.04, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('FNCS', 1, 'Faded Nano Clusters - Superior', 155301, 'CluF', 90, 200, 0.04, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BNCB', 1, 'Bright Nano Clusters - Basic', 155302, 'CluB', 1, 50, 0.04, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BNCA', 1, 'Bright Nano Clusters - Advanced', 155303, 'CluB', 50, 90, 0.04, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('BNCS', 1, 'Bright Nano Clusters - Superior', 155307, 'CluB', 90, 200, 0.04, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SNCB', 1, 'Shining Nano Clusters - Basic', 155308, 'CluS', 1, 50, 0.04, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SNCA', 1, 'Shining Nano Clusters - Advanced', 155309, 'CluS', 50, 90, 0.04, 1, 161);
insert into `vendortemplate` (`HASH`,`lvl`,`Name`,`itemtemplate`,`ShopInvHash`,`minQL`,`maxQL`,`buy`,`sell`,`skill`) VALUES ('SNCS', 1, 'Shining Nano Clusters - Superior', 155310, 'CluS', 90, 200, 0.04, 1, 161);