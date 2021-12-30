drop table userroles;
drop table users;
drop table roles;

CREATE TABLE `roles` (
  `Name` varchar(32) NOT NULL,
  PRIMARY KEY (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(32) NOT NULL,
  `PasswordHash` longblob NOT NULL,
  `PasswordSalt` longblob NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `userroles` (
  `UserId` int(11) NOT NULL,
  `RoleName` varchar(32) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleName`),
  KEY `IX_UserRoles_RoleName` (`RoleName`),
  CONSTRAINT `FK_UserRoles_Roles_RoleName` FOREIGN KEY (`RoleName`) REFERENCES `roles` (`Name`) ON DELETE CASCADE,
  CONSTRAINT `FK_UserRoles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
