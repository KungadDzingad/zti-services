package net.wojtek.zti.services;

import net.wojtek.zti.dao.UserRepository;
import net.wojtek.zti.models.User;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class UserService {

    @Autowired
    private UserRepository userRepo;

    public User getUserById(int id) {
        Optional<User> userOpt = userRepo.findById(id);
        if(userOpt.isEmpty())
            throw new RuntimeException("Bad Creds");
        return userOpt.get();
    }
}
