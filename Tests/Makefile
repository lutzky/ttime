MCS=mcs
CSFLAGS=-codepage:utf8 -r:../UDonkey.exe -pkg:nunit -t:library

TEST_SRCS=DBTests.cs
TEST_LIB=Tests.dll

$(TEST_LIB): $(TEST_SRCS)
	$(MCS) $(CSFLAGS) $(TEST_SRCS) -out:$@

run_tests: $(TEST_LIB)
	nunit-console $(TEST_LIB)

