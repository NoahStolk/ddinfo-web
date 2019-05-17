-- phpMyAdmin SQL Dump
-- version 4.8.4
-- https://www.phpmyadmin.net/
--
-- Host: mysql9.mijnhostingpartner.nl
-- Generation Time: May 17, 2019 at 01:32 PM
-- Server version: 5.6.24-log
-- PHP Version: 7.2.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `stolkdevildaggers`
--

-- --------------------------------------------------------

--
-- Table structure for table `customentries`
--

CREATE TABLE `customentries` (
  `ID` int(11) NOT NULL,
  `Time` float NOT NULL,
  `Gems` int(11) NOT NULL,
  `Kills` int(11) NOT NULL,
  `DeathType` int(11) NOT NULL,
  `EnemiesAlive` int(11) NOT NULL,
  `Homing` int(11) NOT NULL,
  `LevelUpTime2` float NOT NULL,
  `LevelUpTime3` float NOT NULL,
  `LevelUpTime4` float NOT NULL,
  `CustomLeaderboardID` int(11) NOT NULL,
  `SubmitDate` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `ShotsFired` int(11) NOT NULL DEFAULT '0',
  `ShotsHit` int(11) NOT NULL DEFAULT '0',
  `Username` longtext,
  `PlayerID` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `customentries`
--

INSERT INTO `customentries` (`ID`, `Time`, `Gems`, `Kills`, `DeathType`, `EnemiesAlive`, `Homing`, `LevelUpTime2`, `LevelUpTime3`, `LevelUpTime4`, `CustomLeaderboardID`, `SubmitDate`, `ShotsFired`, `ShotsHit`, `Username`, `PlayerID`) VALUES
(1, 51.3827, 3, 18, 1, 791, 0, 0, 0, 0, 1, '2019-05-16 23:25:51.012773', 375, 190, 'xvlv', 21854),
(4, 607.879, 1002, 1069, 10, 64, 0, 28.0664, 92.3719, 155.456, 3, '2019-05-16 22:36:30.120707', 50291, 18527, 'xvlv', 21854),
(5, 35.3996, 10, 39, 8, 341, 0, 0, 0, 0, 1, '2019-05-16 00:33:56.881580', 617, 423, 'Bint', 148788),
(6, 32.633, 1, 59, 5, 303, 0, 0, 0, 0, 1, '2019-05-15 23:44:55.572763', 540, 434, 'Luke', 105315),
(8, 44.5662, 0, 6, 1, 448, 0, 0, 0, 0, 1, '2019-05-16 18:06:03.867739', 124, 24, 'lsai', 187974),
(10, 45.8828, 10, 71, 1, 0, 0, 0, 0, 0, 1, '2019-05-16 17:44:06.788384', 10000, 6917, 'cook', 93991),
(11, 35.083, 0, 11, 1, 386, 0, 0, 0, 0, 1, '2019-05-16 17:50:15.251564', 166, 49, 'Angr', 133050),
(12, 42.3329, 4, 35, 5, 529, 0, 0, 0, 0, 1, '2019-05-16 22:14:50.424964', 433, 197, 'Kasa', 173419),
(13, 35.2996, 3, 94, 3, 293, 0, 0, 0, 0, 1, '2019-05-16 19:00:38.780243', 646, 335, 'Prit', 115431),
(15, 37.2663, 0, 93, 5, 358, 0, 0, 0, 0, 1, '2019-05-16 19:08:06.873632', 778, 582, 'Slay', 88299),
(16, 28.0164, 0, 38, 5, 251, 0, 0, 0, 0, 1, '2019-05-16 18:45:12.846461', 427, 234, 'page', 65617),
(17, 29.9997, 5, 36, 8, 255, 0, 0, 0, 0, 1, '2019-05-16 20:23:33.999355', 437, 345, 'Null', 10782),
(18, 226.539, 239, 406, 5, 115, 0, 0, 0, 0, 3, '2019-05-17 10:10:14.669543', 15764, 5943, 'Luke', 105315),
(19, 444.802, 741, 731, 10, 11, 0, 20.9832, 0, 0, 3, '2019-05-16 22:58:38.658247', 36768, 13441, 'Bint', 148788),
(20, 636.289, 1285, 946, 1, 25, 0, 30.9164, 84.4202, 147.841, 3, '2019-05-17 02:37:04.140408', 54094, 20407, 'lsai', 187974),
(22, 418.759, 664, 662, 12, 35, 0, 10.2167, 51.6161, 121.428, 4, '2019-05-17 13:16:42.293293', 32977, 11132, 'xvlv', 21854);

-- --------------------------------------------------------

--
-- Table structure for table `customleaderboards`
--

CREATE TABLE `customleaderboards` (
  `ID` int(11) NOT NULL,
  `SpawnsetFileName` longtext,
  `SpawnsetHash` longtext,
  `Bronze` float NOT NULL DEFAULT '0',
  `Devil` float NOT NULL DEFAULT '0',
  `Golden` float NOT NULL DEFAULT '0',
  `Silver` float NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `customleaderboards`
--

INSERT INTO `customleaderboards` (`ID`, `SpawnsetFileName`, `SpawnsetHash`, `Bronze`, `Devil`, `Golden`, `Silver`) VALUES
(1, 'AttackAttackAttack_xvlv', '98d52c4f70954bd15dc2ea62d40275cf', 10, 40, 30, 20),
(3, 'bintrs cool spawnset_Bintr', 'e56da368d67fd37fcf57f88df657ab84', 0, 0, 0, 0),
(4, 'HellOnSteroids_xvlv', 'cef48f59e8f948f9fa2d349338c004ef', 80, 400, 280, 160);

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20190515111757_InitialCreate', '2.1.11-servicing-32099'),
('20190515114318_V2', '2.1.11-servicing-32099'),
('20190515115731_V3', '2.1.11-servicing-32099'),
('20190515135521_V4', '2.1.11-servicing-32099'),
('20190515153423_V5', '2.1.11-servicing-32099'),
('20190516104828_V6', '2.1.11-servicing-32099');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `customentries`
--
ALTER TABLE `customentries`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `IX_CustomEntries_CustomLeaderboardID` (`CustomLeaderboardID`);

--
-- Indexes for table `customleaderboards`
--
ALTER TABLE `customleaderboards`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `customentries`
--
ALTER TABLE `customentries`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT for table `customleaderboards`
--
ALTER TABLE `customleaderboards`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `customentries`
--
ALTER TABLE `customentries`
  ADD CONSTRAINT `FK_CustomEntries_CustomLeaderboards_CustomLeaderboardID` FOREIGN KEY (`CustomLeaderboardID`) REFERENCES `customleaderboards` (`ID`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
