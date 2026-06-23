package org.example.studentmanagement.model;

public class Student {
    public String name;
    public Integer age;
    public String major;

    public Student(String name, Integer age, String major) {
        this.name = name;
        this.age = age;
        this.major = major;
    }

    public Integer getAge() {
        return age;
    }

    public String getMajor() {
        return major;
    }

    public String getName() {
        return name;
    }
}
