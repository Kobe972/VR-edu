package com.api.vreduhub.utils;

import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.util.ResourceUtils;
import org.springframework.web.multipart.MultipartFile;

import java.io.File;
import java.io.FileNotFoundException;

@Slf4j
public class FolderUtils {
    @Value("${datapath}")
    private static String datapath;
    public static boolean saveMultiFile(String basePath, MultipartFile[] files) throws Exception{
        if(files==null||files.length==0)
        {
            return false;
        }
        if(basePath.endsWith("/")||basePath.endsWith("\\"))
        {
            basePath = basePath.substring(0, basePath.length()-1);
        }
        for(MultipartFile file:files)
        {
            String filePath = basePath+File.separator+file.getOriginalFilename();

            //File path=new File(ResourceUtils.getURL("classpath:").getPath());
            File _fullPath=new File("D:\\programming\\unity\\VR-edu\\VREduHub\\target\\classes", filePath);
            String fullPath=_fullPath.getAbsolutePath();
            if(File.separator.equals("\\"))
                fullPath = fullPath.replace('/','\\');
            if(File.separator.equals("/"))
                fullPath = fullPath.replace('\\','/');
            try {
                boolean success = makeDir(fullPath);
                if (!success) return false;
                log.info(fullPath);
                file.transferTo(_fullPath);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        return true;
    }
    private static boolean makeDir(String filePath) throws FileNotFoundException
    {
        if(filePath.lastIndexOf(File.separator)>0)
        {
            String dirPath=filePath.substring(0,filePath.lastIndexOf(File.separator));
            File dir=new File(dirPath);
            if(!dir.exists())
            {
                dir.mkdirs();
            }
        }
        return true;
    }
}
