# makephem constructs Chebyshev polynomial files.  readtest reads the ephemerides
# and compares them to test files.

CC = cc

all: makephem readtest

makephem: makephem.o generate.o allocate.o chby.o
	$(CC) -lm makephem.o generate.o allocate.o chby.o -o makephem

readtest: readtest.o readeph.o allocate.o chby.o
	$(CC) -lm readtest.o readeph.o allocate.o chby.o -o readtest

readtest.o: readtest.c
	$(CC) -c readtest.c

readeph.o: readeph.c
	$(CC) -c readeph.c

makephem.o: makephem.c
	$(CC) -c makephem.c

generate.o: generate.c
	$(CC) -c generate.c

allocate.o: allocate.c
	$(CC) -c allocate.c

chby.o: chby.c
	$(CC) -c chby.c

