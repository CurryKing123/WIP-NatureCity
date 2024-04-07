CREATE TABLE `NATURE`.`character_race_test` (
  `idcharacter_race_test` INT NOT NULL,
  `race_name` VARCHAR(45) NULL,
  `race_speed` INT NULL,
  PRIMARY KEY (`idcharacter_race_test`),
  UNIQUE INDEX `idcharacter_race_test_UNIQUE` (`idcharacter_race_test` ASC) VISIBLE);

  ALTER TABLE `NATURE`.`character_race_test` 
CHANGE COLUMN `idcharacter_race_test` `idcharacter_race_test` INT NOT NULL AUTO_INCREMENT ;


INSERT INTO `NATURE`.`character_race_test` (`race_name`, `race_speed`) VALUES ('dog', '3');
