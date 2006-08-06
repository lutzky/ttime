MCS=mcs
CSFLAGS=-codepage:utf8 -pkg:nunit -t:library

TEST_SRCS=DBTests.cs
TEST_LIB=Tests.dll
UDONKEY_DLL=../UDonkey.exe

$(TEST_LIB): $(TEST_SRCS) $(UDONKEY_DLL) 
	$(MCS) $(CSFLAGS) $(TEST_SRCS) -r:$(UDONKEY_DLL) -out:$@

run_tests: $(TEST_LIB)
	nunit-console $(TEST_LIB)

