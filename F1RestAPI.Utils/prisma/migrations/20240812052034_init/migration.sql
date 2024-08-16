/*
  Warnings:

  - The primary key for the `DriverConstructors` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to drop the column `Dnf` on the `Drivers` table. All the data in the column will be lost.
  - You are about to alter the column `DriverCode` on the `Drivers` table. The data in that column could be lost. The data in that column will be cast from `Text` to `VarChar(3)`.
  - Added the required column `DNF` to the `Drivers` table without a default value. This is not possible if the table is not empty.

*/
-- DropForeignKey
ALTER TABLE "DriverConstructors" DROP CONSTRAINT "DriverConstructors_ConstructorId_fkey";

-- DropForeignKey
ALTER TABLE "DriverConstructors" DROP CONSTRAINT "DriverConstructors_DriverId_fkey";

-- AlterTable
CREATE SEQUENCE constructors_constructorid_seq;
ALTER TABLE "Constructors" RENAME CONSTRAINT "Constructors_pkey" TO "PK_Constructors",
ALTER COLUMN "ConstructorId" SET DEFAULT nextval('constructors_constructorid_seq');
ALTER SEQUENCE constructors_constructorid_seq OWNED BY "Constructors"."ConstructorId";

-- AlterTable
ALTER TABLE "DriverConstructors" DROP CONSTRAINT "DriverConstructors_pkey",
RENAME CONSTRAINT "DriverConstructors_pkey" TO "PK_DriverConstructors",
ADD CONSTRAINT "PK_DriverConstructors" PRIMARY KEY ("DriverId", "ConstructorId", "Year");

-- AlterTable
CREATE SEQUENCE drivers_driverid_seq;
ALTER TABLE "Drivers" RENAME CONSTRAINT "Drivers_pkey" TO "PK_Drivers",
DROP COLUMN "Dnf",
ADD COLUMN     "DNF" INTEGER NOT NULL,
ALTER COLUMN "DriverId" SET DEFAULT nextval('drivers_driverid_seq'),
ALTER COLUMN "DriverCode" SET DATA TYPE VARCHAR(3),
ALTER COLUMN "DateOfBirth" SET DATA TYPE TIMESTAMPTZ(6);
ALTER SEQUENCE drivers_driverid_seq OWNED BY "Drivers"."DriverId";

-- CreateTable
CREATE TABLE "IpConnectionCounts" (
    "Ip" TEXT NOT NULL,
    "Count" INTEGER NOT NULL,

    CONSTRAINT "PK_IpConnectionCounts" PRIMARY KEY ("Ip")
);

-- CreateTable
CREATE TABLE "IpCountryStates" (
    "Country" TEXT NOT NULL,
    "City" TEXT NOT NULL,
    "Count" INTEGER NOT NULL,

    CONSTRAINT "PK_IpCountryStates" PRIMARY KEY ("Country","City")
);

-- CreateTable
CREATE TABLE "__EFMigrationsHistory" (
    "MigrationId" VARCHAR(150) NOT NULL,
    "ProductVersion" VARCHAR(32) NOT NULL,

    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

-- CreateIndex
CREATE INDEX "IX_DriverConstructors_ConstructorId" ON "DriverConstructors"("ConstructorId");

-- AddForeignKey
ALTER TABLE "DriverConstructors" ADD CONSTRAINT "FK_DriverConstructors_Constructors_ConstructorId" FOREIGN KEY ("ConstructorId") REFERENCES "Constructors"("ConstructorId") ON DELETE CASCADE ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE "DriverConstructors" ADD CONSTRAINT "FK_DriverConstructors_Drivers_DriverId" FOREIGN KEY ("DriverId") REFERENCES "Drivers"("DriverId") ON DELETE CASCADE ON UPDATE NO ACTION;
