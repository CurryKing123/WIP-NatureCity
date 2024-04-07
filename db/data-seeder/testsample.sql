CREATE TABLE `NATURE`.`test` (
  `idtest` INT NOT NULL,
  `testcol` VARCHAR(45) NULL,
  UNIQUE INDEX `idtest_UNIQUE` (`idtest` ASC) VISIBLE,
  PRIMARY KEY (`idtest`));