FROM debian:wheezy
#FROM frolvlad/alpine-mono

RUN apt-key adv --keyserver pgp.mit.edu --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian wheezy/snapshots/4.0.0 main" > /etc/apt/sources.list.d/mono-xamarin.list


#RUN apt-get update && apt-get install wget ca-certificates -y
#RUN cd /tmp && wget "https://www.archlinux.org/packages/extra/x86_64/mono/download/" -O mono.pkg.tar.xz && \
#    cd / && \
#    tar xJf /tmp/mono.pkg.tar.xz

RUN apt-get update
#RUN apt-get install  mono-devel nuget -y
# RUN apt-get install  mono-complete -y
RUN apt-get install nuget referenceassemblies-pcl -y

#Import the root certificates using the mozroots tool
#RUN mozroots --import --sync
# RUN apk add --update wget ca-certificates; wget http://storage.bos.xamarin.com/bot-provisioning/PortableReferenceAssemblies-2014-04-14.zip -O pcl.zip
#RUN wget "http://www.archlinux.org/packages/extra/any/nuget/download/" -O nuget.pkg.tar.xz && tar xJf nuget.pkg.tar.xz
#RUN wget http://aur.archlinux.org/cgit/aur.git/snapshot/mono-pcl.tar.gz -O pcl.pkg.tar.gz && tar xJf pcl.pkg.tar.gz
#RUN unzip pcl.zip -d /usr/lib/mono/xbuild-frameworks/
ADD . /app
WORKDIR /app
RUN nuget restore ; xbuild XamarinFormsTester.sln