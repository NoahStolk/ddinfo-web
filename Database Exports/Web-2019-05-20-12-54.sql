-- phpMyAdmin SQL Dump
-- version 4.8.4
-- https://www.phpmyadmin.net/
--
-- Host: mysql9.mijnhostingpartner.nl
-- Generation Time: May 20, 2019 at 12:53 PM
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
  `PlayerID` int(11) NOT NULL DEFAULT '0',
  `DDCLClientVersion` varchar(16) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `customentries`
--

INSERT INTO `customentries` (`ID`, `Time`, `Gems`, `Kills`, `DeathType`, `EnemiesAlive`, `Homing`, `LevelUpTime2`, `LevelUpTime3`, `LevelUpTime4`, `CustomLeaderboardID`, `SubmitDate`, `ShotsFired`, `ShotsHit`, `Username`, `PlayerID`, `DDCLClientVersion`) VALUES
(1, 54.6493, 4, 15, 1, 892, 0, 0, 0, 0, 1, '2019-05-17 15:48:48.689749', 686, 312, 'xvlv', 21854, NULL),
(4, 682.511, 1240, 1078, 10, 94, 0, 25.2998, 93.8222, 148.908, 3, '2019-05-19 11:36:06.631702', 59363, 20792, 'xvlv', 21854, NULL),
(5, 35.3996, 10, 39, 8, 341, 0, 0, 0, 0, 1, '2019-05-16 00:33:56.881580', 617, 423, 'Bint', 148788, NULL),
(6, 32.633, 1, 59, 5, 303, 0, 0, 0, 0, 1, '2019-05-15 23:44:55.572763', 540, 434, 'Luke', 105315, NULL),
(8, 44.5662, 0, 6, 1, 448, 0, 0, 0, 0, 1, '2019-05-16 18:06:03.867739', 124, 24, 'lsaille', 187974, NULL),
(10, 52.0494, 11, 47, 3, 735, 0, 37.1329, 0, 0, 1, '2019-05-20 00:57:39.766886', 719, 312, 'cooker', 93991, NULL),
(11, 35.083, 0, 11, 1, 386, 0, 0, 0, 0, 1, '2019-05-16 17:50:15.251564', 166, 49, 'Angr', 133050, NULL),
(12, 42.3329, 4, 35, 5, 529, 0, 0, 0, 0, 1, '2019-05-16 22:14:50.424964', 433, 197, 'Kasa', 173419, NULL),
(13, 35.2996, 3, 94, 3, 293, 0, 0, 0, 0, 1, '2019-05-16 19:00:38.780243', 646, 335, 'Prit', 115431, NULL),
(15, 37.2663, 0, 93, 5, 358, 0, 0, 0, 0, 1, '2019-05-16 19:08:06.873632', 778, 582, 'Slayermanbob', 88299, NULL),
(16, 28.0164, 0, 38, 5, 251, 0, 0, 0, 0, 1, '2019-05-16 18:45:12.846461', 427, 234, 'page', 65617, NULL),
(17, 29.9997, 5, 36, 8, 255, 0, 0, 0, 0, 1, '2019-05-16 20:23:33.999355', 437, 345, 'Nullifier', 10782, NULL),
(18, 226.539, 239, 406, 5, 115, 0, 0, 0, 0, 3, '2019-05-17 10:10:14.669543', 15764, 5943, 'Luke', 105315, NULL),
(19, 444.802, 741, 731, 10, 11, 0, 20.9832, 0, 0, 3, '2019-05-16 22:58:38.658247', 36768, 13441, 'Bint', 148788, NULL),
(20, 636.289, 1285, 946, 1, 25, 0, 30.9164, 84.4202, 147.841, 3, '2019-05-17 02:37:04.140408', 54094, 20407, 'lsaille', 187974, NULL),
(22, 465.064, 703, 850, 4, 140, 0, 7.25004, 35.283, 104.825, 4, '2019-05-17 15:25:15.577585', 38532, 12596, 'xvlv', 21854, NULL),
(23, 480.344, 772, 892, 1, 63, 0, 7.83338, 37.5329, 109.676, 4, '2019-05-20 05:59:47.607740', 43174, 14480, 'lsaille', 187974, NULL),
(24, 435.821, 626, 784, 4, 66, 0, 16.5999, 58.3826, 155.056, 4, '2019-05-20 03:00:43.937562', 37370, 12128, 'Pritster', 115431, NULL),
(25, 264.396, 270, 503, 5, 151, 0, 0, 0, 0, 3, '2019-05-17 16:33:01.571512', 14949, 5270, 'Pritster', 115431, NULL),
(26, 77.4687, 114, 86, 10, 4, 0, 0, 0, 0, 4, '2019-05-17 22:15:50.546657', 2568, 869, 'Luke', 105315, NULL),
(27, 858.018, 821, 2398, 1, 141, 0, 57.4493, 125.396, 278.193, 5, '2019-05-19 15:11:42.593960', 74286, 24975, 'xvlv', 21854, NULL),
(28, 920.269, 1069, 2954, 1, 24, 0, 53.1994, 138.977, 228.572, 5, '2019-05-20 05:14:28.590189', 81188, 27567, 'lsaille', 187974, NULL),
(33, 33.033, 8, 48, 2, 12, 0, 0, 0, 0, 6, '2019-05-19 13:18:38.268735', 404, 100, 'xvlv', 21854, NULL),
(34, 48.0828, 75, 3, 0, 3, 5, 12.7833, 39.6162, 0, 8, '2019-05-19 13:20:58.866760', 886, 225, 'xvlv', 21854, NULL),
(35, 824.759, 799, 1964, 3, 0, 0, 0, 0, 0, 5, '2019-05-18 16:59:00.860831', 70790, 22697, 'Pritster', 115431, NULL),
(36, 165.837, 178, 402, 1, 48, 0, 30.2497, 129.829, 0, 3, '2019-05-18 21:41:12.845294', 5746, 2550, 'Nullifier', 10782, NULL),
(37, 580.986, 452, 1591, 15, 41, 0, 60.7659, 127.929, 276.96, 5, '2019-05-19 16:59:35.556006', 45001, 15183, 'Kasa', 173419, NULL),
(38, 579.069, 537, 1837, 15, 41, 0, 55.4993, 126.829, 247.467, 5, '2019-05-19 06:56:50.301959', 42598, 15371, 'jay', 105641, NULL),
(39, 42.4495, 4, 30, 8, 535, 0, 0, 0, 0, 1, '2019-05-19 10:34:13.136629', 585, 256, 'Nito', 111007, NULL),
(40, 381.218, 276, 1618, 6, 37, 0, 0, 0, 0, 5, '2019-05-19 19:01:54.504478', 18023, 5927, 'Nito', 111007, NULL),
(41, 305.803, 780, 15, 0, 0, 0, 0, 0, 0, 8, '2019-05-19 16:31:10.407359', 19630, 4198, 'Alk', 182130, NULL),
(42, 76.9686, 34, 269, 9, 53, 0, 0, 0, 0, 3, '2019-05-19 16:36:17.510707', 1933, 691, 'Alk', 182130, NULL),
(43, 391.982, 273, 1018, 1, 20, 0, 0, 0, 0, 5, '2019-05-19 17:38:57.806862', 17131, 5784, 'Stop.', 148951, NULL),
(44, 953.511, 933, 1734, 9, 65, 0, 23.7498, 94.2056, 261.114, 9, '2019-05-19 23:50:25.261483', 82174, 27990, 'Kasa', 173419, NULL),
(45, 1243.14, 1490, 2426, 11, 121, 0, 23.8998, 93.0387, 258.248, 9, '2019-05-19 17:43:39.835784', 106141, 33213, 'lsaille', 187974, NULL),
(46, 1257.72, 1633, 2444, 1, 34, 0, 28.7997, 98.5732, 252.782, 9, '2019-05-19 17:32:40.273521', 100028, 33977, 'xvlv', 21854, NULL),
(47, 1237.07, 1689, 2318, 3, 34, 0, 27.8997, 92.7887, 254.199, 9, '2019-05-19 18:24:26.579832', 99672, 32787, 'Pritster', 115431, NULL),
(48, 119.194, 0, 0, 10, 20, 0, 0, 0, 0, 10, '2019-05-19 19:13:27.301703', 0, 0, 'lsaille', 187974, NULL),
(49, 85.7205, 0, 0, 10, 14, 0, 0, 0, 0, 10, '2019-05-19 18:53:17.173284', 0, 0, 'xvlv', 21854, NULL),
(50, 63.4992, 180, 1, 10, 2, 0, 15.1999, 38.3329, 0, 10, '2019-05-19 18:19:03.518296', 2344, 817, 'cooker', 93991, NULL),
(51, 69.6671, 123, 0, 10, 0, 0, 14.7833, 51.0994, 0, 10, '2019-05-19 19:02:04.143031', 1106, 585, 'Kasa', 173419, NULL),
(52, 47.6828, 0, 0, 10, 7, 0, 0, 0, 0, 10, '2019-05-20 03:24:08.717293', 0, 0, 'Pritster', 115431, NULL),
(53, 28.0831, 0, 37, 8, 237, 0, 0, 0, 0, 1, '2019-05-19 19:54:49.869251', 486, 457, 'Goobermin', 89987, NULL),
(54, 444.386, 352, 1583, 15, 39, 0, 61.7992, 128.546, 271.628, 5, '2019-05-19 20:18:58.557734', 29930, 9430, 'Goobermin', 89987, NULL),
(55, 68.8669, 211, 0, 10, 10, 10, 15.0666, 42.4329, 0, 10, '2019-05-19 20:40:02.584379', 2329, 840, 'Goobermin', 89987, NULL),
(56, 81.6196, 0, 0, 10, 5, 0, 0, 0, 0, 10, '2019-05-19 20:24:59.133252', 0, 0, 'Stop.', 148951, NULL),
(57, 37.1163, 3, 72, 5, 357, 0, 0, 0, 0, 1, '2019-05-19 21:00:52.168263', 689, 430, 'gLad', 134802, NULL),
(58, 272.428, 117, 759, 1, 14, 0, 0, 0, 0, 6, '2019-05-19 20:43:45.883906', 9227, 1645, 'Stop.', 148951, NULL),
(59, 203.711, 300, 309, 0, 2, 0, 19.8832, 46.8328, 125.596, 4, '2019-05-19 21:16:57.920933', 13513, 4771, 'gLad', 134802, NULL),
(60, 92.6053, 147, 1, 10, 13, 16, 19.4999, 36.1996, 0, 10, '2019-05-19 21:54:53.915400', 1442, 658, 'pagedMov', 65617, NULL),
(61, 46.0828, 1, 82, 3, 587, 0, 0, 0, 0, 1, '2019-05-19 22:54:05.498902', 464, 194, 'pocket', 106722, NULL),
(62, 327.598, 284, 797, 1, 8, 0, 0, 0, 0, 9, '2019-05-19 22:51:42.269137', 18215, 6795, 'Stop.', 148951, NULL),
(63, 707.005, 1337, 1206, 10, 54, 0, 19.5999, 87.6376, 146.142, 3, '2019-05-19 23:56:08.666797', 59449, 22405, 'pocket', 106722, NULL),
(64, 483.159, 774, 905, 10, 44, 0, 8.03338, 36.333, 111.476, 4, '2019-05-20 00:22:19.621194', 40415, 14284, 'pocket', 106722, NULL),
(65, 86.2039, 26, 0, 10, 14, 0, 23.4331, 0, 0, 10, '2019-05-20 00:42:35.971765', 702, 198, 'pocket', 106722, NULL),
(66, 307.119, 204, 782, 1, 45, 0, 0, 0, 0, 5, '2019-05-20 00:38:01.154378', 10917, 3086, 'cooker', 93991, NULL),
(68, 808.197, 876, 2572, 1, 48, 0, 52.2994, 134.761, 230.288, 5, '2019-05-20 01:28:58.677197', 69469, 23862, 'pocket', 106722, NULL),
(69, 324.015, 160, 1012, 1, 38, 0, 36.7163, 180.067, 0, 6, '2019-05-20 03:08:09.302599', 12712, 2165, 'Pritster', 115431, NULL),
(70, 361.672, 1221, 26, 0, 0, 0, 12.7, 37.8329, 97.9898, 8, '2019-05-20 03:20:50.451021', 14403, 4814, 'Pritster', 115431, NULL),
(71, 375.919, 181, 1008, 11, 17, 0, 48.0994, 148.891, 0, 5, '2019-05-20 10:48:11.031875', 17676, 5158, 'Alk', 182130, NULL),
(72, 49.2161, 61, 0, 10, 1, 0, 15.3666, 0, 0, 10, '2019-05-20 10:53:04.397587', 1256, 383, 'Alk', 182130, NULL),
(73, 233.32, 163, 573, 15, 16, 0, 29.0164, 103.007, 0, 9, '2019-05-20 11:07:09.958988', 11441, 3402, 'Alk', 182130, NULL);

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
(4, 'HellOnSteroids_xvlv', 'cef48f59e8f948f9fa2d349338c004ef', 80, 400, 280, 160),
(5, 'TheFourLeviathanOfTheApocalypse_lsaille', '3a3a552d2a00b945eca380f4f4d9b0f7', 200, 1000, 666, 400),
(6, 'Aim_Training_Cookie', '26d6b16d5ee7a5eea797771c67e5aae6', 100, 325, 250, 180),
(8, 'Gigapedes_xvlv', '8f89db7a75406040439dd87ac8c4a8e5', 0, 0, 0, 0),
(9, 'FourDigitSkullSplitter_Pritster', 'ef00a85da30cb93938f9e523b43d1693', 300, 1000, 750, 500),
(10, 'Coil_lsaille', '96f123d08b06b6ee9d5233d5df39d7f7', 25, 65, 50, 40);

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
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=74;

--
-- AUTO_INCREMENT for table `customleaderboards`
--
ALTER TABLE `customleaderboards`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

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
