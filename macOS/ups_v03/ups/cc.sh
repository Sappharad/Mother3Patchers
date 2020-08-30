clear
g++ -c ups.cpp -I. -I..
g++ -c libups/libups.cpp -I. -I..
g++ -c ../hiro/hiro.cpp `pkg-config gtk+-2.0 --cflags` -I. -I..
g++ -c ../nall/string.cpp -I. -I..
g++ -o ups ups.o libups.o hiro.o string.o `pkg-config gtk+-2.0 --libs` -lXtst
strip -s ups
rm *.o
