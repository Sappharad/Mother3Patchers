mingw32-g++ -c ups.cpp -I. -I..
mingw32-g++ -c libups/libups.cpp -I. -I..
mingw32-g++ -c ../hiro/hiro.cpp -I. -I..
mingw32-g++ -c ../nall/string.cpp -I. -I..
windres ups.rc upsrc.o
mingw32-g++ -o ups.exe ups.o upsrc.o libups.o hiro.o string.o -lkernel32 -luser32 -lgdi32 -ladvapi32 -lcomdlg32 -lcomctl32 -mwindows
strip -s ups.exe
@pause
@del *.o
