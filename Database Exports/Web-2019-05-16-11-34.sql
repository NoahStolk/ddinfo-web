-- phpMyAdmin SQL Dump
-- version 4.8.4
-- https://www.phpmyadmin.net/
--
-- Host: mysql9.mijnhostingpartner.nl
-- Generation Time: May 16, 2019 at 11:34 AM
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
(1, 50.5661, 0, 32, 5, 769, 0, 0, 0, 0, 1, '2019-05-16 00:02:27.495085', 332, 105, 'xvlv', 21854),
(4, 6.53336, 1, 0, 0, 24, 0, 0, 0, 0, 3, '2019-05-15 23:32:49.792424', 20, 10, 'xvlv', 21854),
(5, 35.3996, 10, 39, 8, 341, 0, 0, 0, 0, 1, '2019-05-16 00:33:56.881580', 617, 423, 'Bint', 148788),
(6, 32.633, 1, 59, 5, 303, 0, 0, 0, 0, 1, '2019-05-15 23:44:55.572763', 540, 434, 'Luke', 105315),
(7, 0, 4126848, 1, 1986354293, 201326604, 0, 29.4331, 0, 0, 1, '2019-05-16 01:20:22.451130', 1986356591, 8098840, '�{\0', 7798784);

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
  `Gold` float NOT NULL DEFAULT '0',
  `Silver` float NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `customleaderboards`
--

INSERT INTO `customleaderboards` (`ID`, `SpawnsetFileName`, `SpawnsetHash`, `Bronze`, `Devil`, `Gold`, `Silver`) VALUES
(1, 'AttackAttackAttack_xvlv', '98d52c4f70954bd15dc2ea62d40275cf', 10, 40, 30, 20),
(3, 'bintrs cool spawnset_Bintr', 'e56da368d67fd37fcf57f88df657ab84', 0, 0, 0, 0);

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
('20190515153423_V5', '2.1.11-servicing-32099');

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
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `customleaderboards`
--
ALTER TABLE `customleaderboards`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

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
