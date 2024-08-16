-- CreateTable
CREATE TABLE "Drivers" (
    "DriverId" INTEGER NOT NULL,
    "DriverRef" TEXT NOT NULL,
    "DriverNumber" INTEGER NOT NULL,
    "DriverCode" TEXT NOT NULL,
    "DriverForename" TEXT NOT NULL,
    "DriverSurname" TEXT NOT NULL,
    "DateOfBirth" TIMESTAMP(3) NOT NULL,
    "Nationality" TEXT NOT NULL,
    "WikipediaUrl" TEXT NOT NULL,
    "PolePositions" INTEGER NOT NULL,
    "Wins" INTEGER NOT NULL,
    "Podiumns" INTEGER NOT NULL,
    "Dnf" INTEGER NOT NULL,

    CONSTRAINT "Drivers_pkey" PRIMARY KEY ("DriverId")
);

-- CreateTable
CREATE TABLE "Constructors" (
    "ConstructorId" INTEGER NOT NULL,
    "ConstructorRef" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Nationality" TEXT NOT NULL,
    "Url" TEXT NOT NULL,

    CONSTRAINT "Constructors_pkey" PRIMARY KEY ("ConstructorId")
);

-- CreateTable
CREATE TABLE "DriverConstructors" (
    "Year" INTEGER NOT NULL,
    "DriverId" INTEGER NOT NULL,
    "ConstructorId" INTEGER NOT NULL,

    CONSTRAINT "DriverConstructors_pkey" PRIMARY KEY ("Year","ConstructorId","DriverId")
);

-- AddForeignKey
ALTER TABLE "DriverConstructors" ADD CONSTRAINT "DriverConstructors_DriverId_fkey" FOREIGN KEY ("DriverId") REFERENCES "Drivers"("DriverId") ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "DriverConstructors" ADD CONSTRAINT "DriverConstructors_ConstructorId_fkey" FOREIGN KEY ("ConstructorId") REFERENCES "Constructors"("ConstructorId") ON DELETE RESTRICT ON UPDATE CASCADE;
